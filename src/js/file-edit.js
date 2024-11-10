import { MAX_FILE_TEXT_LENGTH, MAX_FILE_TITLE_LENGTH } from "./const.js";

const fileEditTitleInput = document.querySelector('.input__edit-file__title');
const fileEditTextInput = document.querySelector('.input__edit-file__text');
const closeFileEditingBtn = document.querySelector('.close__file-editing');
const editFileModal = document.querySelector('.edit__file');
const saveEditFileButton = document.querySelector('.file__action-edit-btn__save');
const deleteEditFileButton = document.querySelector('.file__action-edit-btn__delete');
const cancelDeleteEditFileButton = document.querySelector('.approve__cancel');
const approveOpen = document.querySelector('.approve__open');

//Ограничение количества символов в заголовке и в описании
if (fileEditTitleInput) {
   fileEditTitleInput.addEventListener('input', () => {
      if (fileEditTitleInput.value.length > MAX_FILE_TITLE_LENGTH) {
         fileEditTitleInput.value = fileEditTitleInput.value.slice(0, MAX_FILE_TITLE_LENGTH);
      }
   });
}

if (fileEditTextInput) {
   fileEditTextInput.addEventListener('input', () => {
      if (fileEditTextInput.value.length > MAX_FILE_TEXT_LENGTH) {
         fileEditTextInput.value = fileEditTextInput.value.slice(0, MAX_FILE_TEXT_LENGTH);
      }
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
    if (fileEditTitleInput.value.length > 0 && fileEditTextInput.value.length > 0) {
        saveEditFileButton.classList.remove('isDisabled');
    } else {
        saveEditFileButton.classList.add('isDisabled');
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
