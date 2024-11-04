import { MAX_TITLE_LENGTH } from "./const.js";

const titleInput = document.querySelector('.input__title');

//Ограничение количества символов в заголовке
titleInput.addEventListener('input', () => {
   if (titleInput.value.length > MAX_TITLE_LENGTH) {
      titleInput.value = titleInput.value.slice(0, MAX_TITLE_LENGTH);
   }
});

