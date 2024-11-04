import { MAX_TITLE_LENGTH } from "./const.js";

const titleInput = document.querySelector('.input__title');
const fileInput = document.querySelector('.add__file-input');
const fileLinkContainer = document.querySelector('.file__link');
const fileOpenLink = document.querySelector('.file__open-link');
const fileDeleteButton = document.querySelector('.file__delete-button');


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

