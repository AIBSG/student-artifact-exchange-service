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

//Ограничение количества символов в заголовке и в описании
if (fileTitleInput) {
   fileTitleInput.addEventListener('input', () => {
      if (fileTitleInput.value.length > MAX_FILE_TITLE_LENGTH) {
         fileTitleInput.value = fileTitleInput.value.slice(0, MAX_FILE_TITLE_LENGTH);
      }
   });
}

if (fileTextInput) {
   fileTextInput.addEventListener('input', () => {
      if (fileTextInput.value.length > MAX_FILE_TEXT_LENGTH) {
         fileTextInput.value = fileTextInput.value.slice(0, MAX_FILE_TEXT_LENGTH);
      }
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
      if (fileTitleInput.value.length > 0 && fileTextInput.value.length > 0 && createFileInput.files.length > 0) {
         saveCreateFileButton.classList.remove('isDisabled');
      } else {
         saveCreateFileButton.classList.add('isDisabled');
      }
   }
}

toggleSaveButton();

if (fileTitleInput && fileTextInput && createFileInput) {
   fileTitleInput.addEventListener('input', toggleSaveButton);
   fileTextInput.addEventListener('input', toggleSaveButton);
   createFileInput.addEventListener('change', toggleSaveButton);
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