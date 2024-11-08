import { MAX_TITLE_LENGTH } from "./const.js";

const titleInput = document.querySelector('.input__title');
const textInput = document.querySelector('.input__text');
const fileInput = document.querySelector('.add__file-input');
const fileLinkContainer = document.querySelector('.file__link');
const fileOpenLink = document.querySelector('.file__open-link');
const fileDeleteButton = document.querySelector('.file__delete-button');
const saveNoteButton = document.querySelector('.save');


//Ограничение количества символов в заголовке
titleInput.addEventListener('input', () => {
   if (titleInput.value.length > MAX_TITLE_LENGTH) {
      titleInput.value = titleInput.value.slice(0, MAX_TITLE_LENGTH);
   }
});

//Загрузка файла в заметку и возможность его открыть и просмотреть
fileInput.addEventListener('change', () => {
   const file = fileInput.files[0];

   const displayFileName = file.name.length > 15 ? file.name.slice(0, 12) + '...' : file.name;

   if (file) {
      const fileURL = URL.createObjectURL(file);
      
      fileOpenLink.href = fileURL;
      fileOpenLink.textContent = `${displayFileName}`;
      fileLinkContainer.classList.remove('hidden');
   }
});

fileDeleteButton.addEventListener('click', () => {
   fileInput.value = '';

   fileOpenLink.href = '#';
   fileOpenLink.textContent = '';
   fileLinkContainer.classList.add('hidden');
});

//Если заголовок и текст заметки пусты, кнопка сохранить недоступна
titleInput.addEventListener('input', toggleSaveButton);
textInput.addEventListener('input', toggleSaveButton);

function toggleSaveButton() {
    if (titleInput.value.length > 0 && textInput.value.length > 0) {
        saveNoteButton.classList.remove('isDisabled');
    } else {
        saveNoteButton.classList.add('isDisabled');
    }
}
