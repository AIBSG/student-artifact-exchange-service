import { MAX_FILE_TEXT_LENGTH, MAX_FILE_TITLE_LENGTH } from "./const.js";
import { openShareModal, closeShareModal } from "./modal-open.js";
import { API_FILE_EDIT_URL } from "./api.js";

const fileEditTitleInput = document.querySelector('.input__edit-file__title');
const fileEditTextInput = document.querySelector('.input__edit-file__text');
const closeFileEditingBtn = document.querySelector('.close__file-editing');
const editFileModal = document.querySelector('.edit__file');
const saveEditFileButton = document.querySelector('.file__action-edit-btn__save');
const deleteEditFileButton = document.querySelector('.file__action-edit-btn__delete');
const cancelDeleteEditFileButton = document.querySelector('.approve__cancel');
const approveOpen = document.querySelector('.approve__open');
const shareButton = editFileModal.querySelector('.file__share-button');
const cancelButton = document.querySelector('.share__cancel');
const fileEditTagInput = document.querySelector('#tag');

// Функция для отправки данных на бэкенд
async function sendFileEditData() {
   const title = fileEditTitleInput.value.trim();
   const text = fileEditTextInput.value.trim();
   const tag = fileEditTagInput.value.trim();
   const shareData = getFileEditShareData();

   if (!title || !text || !tag) {
       alert('Все поля должны быть заполнены!');
       return;
   }

   const formData = new FormData();
    formData.append('title', title);
    formData.append('text', text);
    formData.append('tag', tag);

    if (shareData) {
        formData.append('shareData', JSON.stringify(shareData)); // Передаём данные "Поделиться" как JSON
    }

   try {
       const response = await fetch(API_FILE_EDIT_URL, {
           method: 'PUT', // Используем метод PUT для обновления данных
           headers: {
               'Content-Type': 'application/json',
           },
           body: JSON.stringify(formData),
       });

       if (!response.ok) {
           throw new Error(`Ошибка: ${response.statusText}`);
       }

       const result = await response.json();
       alert(result.message || 'Данные успешно обновлены.');
       editFileModal.classList.add('hidden');
   } catch (error) {
       console.error('Ошибка при отправке данных:', error);
       alert('Не удалось сохранить изменения. Попробуйте снова.');
   }
}

// Функция для получения данных из окна "Поделиться"
function getFileEditShareData() {
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
if (saveEditFileButton) {
   saveEditFileButton.addEventListener('click', (event) => {
       if (saveEditFileButton.classList.contains('isDisabled')) {
           event.preventDefault();
           return;
       }
       sendFileEditData();
   });
}

// Функция для сохранения данных в localStorage
function saveFileEditData() {
   const fileEditData = {
       title: fileEditTitleInput.value,
       text: fileEditTextInput.value,
       tag: fileEditTagInput.value,
   };
   localStorage.setItem('fileEditData', JSON.stringify(fileEditData));
}

// Функция для загрузки данных из localStorage
function loadFileEditData() {
   const savedFileEditData = localStorage.getItem('fileEditData');
   if (savedFileEditData) {
       const data = JSON.parse(savedFileEditData);

       if (fileEditTitleInput) {
        fileEditTitleInput.value = data.title || '';
       }

       if (fileEditTextInput) {
        fileEditTextInput.value = data.text || '';
       }

       if (fileEditTagInput) {
        fileEditTagInput.value = data.tag || '';
       }
   }
   toggleSaveButton();
}

// Ограничение количества символов в заголовке и в описании
if (fileEditTitleInput) {
   fileEditTitleInput.addEventListener('input', () => {
       if (fileEditTitleInput.value.length > MAX_FILE_TITLE_LENGTH) {
           fileEditTitleInput.value = fileEditTitleInput.value.slice(0, MAX_FILE_TITLE_LENGTH);
       }
       saveFileEditData();
   });
}

if (fileEditTextInput) {
   fileEditTextInput.addEventListener('input', () => {
       if (fileEditTextInput.value.length > MAX_FILE_TEXT_LENGTH) {
           fileEditTextInput.value = fileEditTextInput.value.slice(0, MAX_FILE_TEXT_LENGTH);
       }
       saveFileEditData();
   });
}

if (fileEditTagInput) {
   fileEditTagInput.addEventListener('input', saveFileEditData);
}

//Обработчик закрытия окна создания заметки-файла
if (closeFileEditingBtn) {
   closeFileEditingBtn.addEventListener('click', () => {
      editFileModal.classList.add('hidden');
   });
}

// Сохранение данных при нажатии на кнопку "Сохранить"
if (saveEditFileButton) {
   saveEditFileButton.addEventListener('click', (event) => {
       if (saveEditFileButton.classList.contains('isDisabled')) {
           event.preventDefault();
           return;
       }
       sendFileEditData();
   });
}

// Если заголовок, текст и тег пусты, кнопка "Сохранить" недоступна
function toggleSaveButton() {
   if (fileEditTitleInput && fileEditTextInput && fileEditTagInput) {
       if (fileEditTitleInput.value.length > 0 && fileEditTextInput.value.length > 0 && fileEditTagInput.value.length > 0) {
           saveEditFileButton.classList.remove('isDisabled');
       } else {
           saveEditFileButton.classList.add('isDisabled');
       }
   }
}

// Открытие и закрытие окна с подтверждением удаления заметки-файла
if (deleteEditFileButton) {
   deleteEditFileButton.addEventListener('click', () => {
       approveOpen.classList.remove('hidden');
   });
}

if (cancelDeleteEditFileButton) {
   cancelDeleteEditFileButton.addEventListener('click', () => {
       approveOpen.classList.add('hidden');
   });
}

// Открытие и закрытие окна с возможностью поделиться заметкой
if (shareButton) {
   shareButton.addEventListener('click', openShareModal);
}

if (cancelButton) {
   cancelButton.addEventListener('click', closeShareModal);
}

document.addEventListener('DOMContentLoaded', () => {
    loadFileEditData();
 });
