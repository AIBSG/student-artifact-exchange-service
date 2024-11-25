import {MAX_FILE_TITLE_LENGTH, MAX_FILE_TEXT_LENGTH} from './const.js';
import { openShareModal, closeShareModal } from './modal-open.js';

const fileTitleInput = document.querySelector('.input__file-title');
const fileTextInput = document.querySelector('.input__file-text');
const createFileModal = document.querySelector('.create__file');
const closeFileCreatingBtn = document.querySelector('.close__file-creating');
const createFileInput = document.querySelector('.add__file-input');
const createFileLinkContainer = document.querySelector('.file__link');
const createFileOpenLink = document.querySelector('.file__open-link');
const saveCreateFileButton = document.querySelector('.file__action-create-btn__save');
const createFileDeleteButton = document.querySelector('.file__delete-button');
const shareButton = createFileModal.querySelector('.file__share-button');
const cancelButton = document.querySelector('.share__cancel');
const fileTagInput = document.querySelector('#tag');

// Функция для сохранения данных в localStorage
function saveFileData() {
   const fileData = {
       title: fileTitleInput.value,
       text: fileTextInput.value,
       tag: fileTagInput.value 
   };
   localStorage.setItem('fileData', JSON.stringify(fileData));
}
// Функция для загрузки данных из localStorage
function loadFileData() {
  const savedFileData = localStorage.getItem('fileData');
  if (savedFileData) {
     const data = JSON.parse(savedFileData);
     fileTitleInput.value = data.title;
     fileTextInput.value = data.text;
     fileTagInput.value = data.tag;
  }
  toggleSaveButton(); 
}

//Ограничение количества символов в заголовке и в описании
if (fileTitleInput) {
   fileTitleInput.addEventListener('input', () => {
      if (fileTitleInput.value.length > MAX_FILE_TITLE_LENGTH) {
         fileTitleInput.value = fileTitleInput.value.slice(0, MAX_FILE_TITLE_LENGTH);
      }
      saveFileData()
   });
}

if (fileTextInput) {
   fileTextInput.addEventListener('input', () => {
      if (fileTextInput.value.length > MAX_FILE_TEXT_LENGTH) {
         fileTextInput.value = fileTextInput.value.slice(0, MAX_FILE_TEXT_LENGTH);
      }
      saveFileData()
   });
}

if (fileTagInput) {
   fileTagInput.addEventListener('input', () => {
       saveFileData();
   });
}

//Обработчик закрытия окна создания заметки-файла
if (closeFileCreatingBtn) {
   closeFileCreatingBtn.addEventListener('click', () => {
      createFileModal.classList.add('hidden');
   });
}

//Загрузка файла в заметку и возможность его открыть и просмотреть
createFileInput.addEventListener('change', () => {
   const file = createFileInput.files[0];

   const displayFileName = file.name.length > 15 ? file.name.slice(0, 12) + '...' : file.name;

   if (file) {
      const fileURL = URL.createObjectURL(file);
      
      createFileOpenLink.href = fileURL;
      createFileOpenLink.textContent = `${displayFileName}`;
      createFileLinkContainer.classList.remove('hidden');
   }
});

//Если заголовок, текст и поле файла заметки-файла пусты, кнопка сохранить недоступна
function toggleSaveButton() {
   if (fileTitleInput && fileTextInput && createFileInput) {
      if (fileTitleInput.value.length > 0 && fileTextInput.value.length > 0 && createFileInput.files.length > 0 && fileTagInput.value.length > 0) {
         saveCreateFileButton.classList.remove('isDisabled');
      } else {
         saveCreateFileButton.classList.add('isDisabled');
      }
   }
}

toggleSaveButton();

if (fileTitleInput && fileTextInput && createFileInput && fileTagInput) {
   fileTitleInput.addEventListener('input', toggleSaveButton);
   fileTextInput.addEventListener('input', toggleSaveButton);
   createFileInput.addEventListener('change', toggleSaveButton);
   fileTagInput.addEventListener('change', toggleSaveButton);
}

if (createFileDeleteButton) {
   createFileDeleteButton.addEventListener('click', () => {
      createFileInput.value = '';
      createFileOpenLink.href = '#';
      createFileOpenLink.textContent = '';
      createFileLinkContainer.classList.add('hidden');
      toggleSaveButton(); 
   });
}


//Открытие и закрытие окна с возможностью поделиться заметкой
if (shareButton) {
   shareButton.addEventListener('click', openShareModal)
}

if (cancelButton) {
   cancelButton.addEventListener('click', closeShareModal)
}

loadFileData();