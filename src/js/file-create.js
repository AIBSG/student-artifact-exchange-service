import {MAX_FILE_TITLE_LENGTH, MAX_FILE_TEXT_LENGTH} from './const.js';
import { openShareModal, closeShareModal } from './modal-open.js';
import { API_FILE_CREATE_URL } from './api.js';

const fileTitleInput = document.querySelector('.input__file-title');
const fileTextInput = document.querySelector('.input__file-text');
const createFileModal = document.querySelector('.create__file');
const closeFileCreatingBtn = document.querySelector('.close__file-creating');
const createFileInput = document.querySelector('.add__file-input');
const createFileLinkContainer = document.querySelector('.file__link');
const createFileOpenLink = document.querySelector('.file__open-link');
const saveCreateFileButton = document.querySelector('.file__action-create-btn__save');
const createFileDeleteButton = document.querySelector('.file__delete-button');
const shareButton = document.querySelector('.file__share-button');
const cancelButton = document.querySelector('.share__cancel');
const fileTagInput = document.querySelector('#tag');


// Функция для отправки данных на бэкенд
async function sendFileData() {
   const fileTitle = fileTitleInput.value.trim();
   const fileText = fileTextInput.value.trim();
   const fileTag = fileTagInput.value.trim();
   const fileFile = createFileInput.files[0];
   const fileShareData = getFileShareData(); // Получение данных из окна "Поделиться"

   if (!fileTitle || !fileText || !fileFile || !fileTag) {
       alert('Все поля должны быть заполнены!');
       return;
   }

   const fileFormData = new FormData();
   fileFormData.append('title', fileTitle);
   fileFormData.append('text', fileText);
   fileFormData.append('tag', fileTag);
   fileFormData.append('file', fileFile);

   if (fileShareData) {
       fileFormData.append('fileShareData', JSON.stringify(fileShareData)); // Передаём данные "Поделиться" как JSON
   }

   try {
       const response = await fetch(API_FILE_CREATE_URL, {
           method: 'POST',
           body: fileFormData,
       });

       if (!response.ok) {
           throw new Error(`Ошибка: ${response.statusText}`);
       }

       const result = await response.json();
       alert(result.message || 'Файл успешно сохранён.');
       closeFileModal();
   } catch (error) {
       console.error('Ошибка при отправке данных:', error);
       alert('Не удалось сохранить данные. Попробуйте снова.');
   }
}

// Функция для получения данных из окна "Поделиться"
function getFileShareData() {
   const email = document.querySelector('.input__email')?.value.trim();
   const accessMode = document.querySelector('.share__options')?.value;
   const isReadForAllChecked = document.querySelector('.input__checkbox')?.checked;

   return {
       email: email || null,
       accessMode: accessMode || null,
       readForAll: isReadForAllChecked || false,
   };
}

// Сохранение данных при нажатии на кнопку "Сохранить"
if (saveCreateFileButton) {
   saveCreateFileButton.addEventListener('click', (event) => {
       if (saveCreateFileButton.classList.contains('isDisabled')) {
           event.preventDefault();
           return;
       }
       sendFileData();
   });
}

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

      if (fileTitleInput) {
         fileTitleInput.value = data.title || '';
      }
      if (fileTextInput) {
         fileTextInput.value = data.text || '';
      }
      if (fileTagInput) {
         fileTagInput.value = data.tag || '';
      }
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

   const displayFileName = file.name.length > 15 ? file.name.slice(0, 15) + '...' : file.name;

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

document.addEventListener('DOMContentLoaded', () => {
   loadFileData();
});