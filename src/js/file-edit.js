import { MAX_FILE_TEXT_LENGTH, MAX_FILE_TITLE_LENGTH } from "./const.js";
import { openShareModal, closeShareModal } from "./modal-open.js";

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


// Функция для сохранения данных в localStorage
function saveFileEditData() {
   const fileEditData = {
       title: fileEditTitleInput.value,
       text: fileEditTextInput.value,
       tag: fileEditTagInput.value 
   };
   localStorage.setItem('fileEditData', JSON.stringify(fileEditData));
}

// Функция для загрузки данных из localStorage
function loadFileEditData() {
  const savedFileEditData = localStorage.getItem('fileEditData');
  if (savedFileEditData) {
     const data = JSON.parse(savedFileEditData);
     fileEditTitleInput.value = data.title;
     fileEditTextInput.value = data.text;
     fileEditTagInput.value = data.tag;
  }
  toggleSaveButton(); 
}

//Ограничение количества символов в заголовке и в описании
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
   fileEditTagInput.addEventListener('input', () => {
       saveFileEditData(); // Сохраняем данные после изменения текста
   });
}

//Обработчик закрытия окна создания заметки-файла
if (closeFileEditingBtn) {
   closeFileEditingBtn.addEventListener('click', () => {
      editFileModal.classList.add('hidden');
   });
}

//Если заголовок и текст заметки пусты, кнопка сохранить недоступна
if (fileEditTitleInput && fileEditTextInput) {
   fileEditTitleInput.addEventListener('input', toggleSaveButton);
   fileEditTextInput.addEventListener('input', toggleSaveButton);
}

function toggleSaveButton() {
   if (fileEditTitleInput && fileEditTextInput) {
      if (fileEditTitleInput.value.length > 0 && fileEditTextInput.value.length > 0) {
         saveEditFileButton.classList.remove('isDisabled');
      } else {
         saveEditFileButton.classList.add('isDisabled');
      }
   }
}

//Окрытие и закрытие окна с подтверждением удаления заметки-файла
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

//Открытие и закрытие окна с возможностью поделиться заметкой
if (shareButton) {
   shareButton.addEventListener('click', openShareModal)
}

if (cancelButton) {
   cancelButton.addEventListener('click', closeShareModal)
}

loadFileEditData();
