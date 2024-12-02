import { MAX_TITLE_LENGTH } from "./const.js";
import { API_NOTE_CREATE_URL } from "./api.js";

const noteTitleInput = document.querySelector('.input__note-title');
const noteTextInput = document.querySelector('.input__note-text');
const noteFileInput = document.querySelector('.add__file-input');
const fileLinkContainer = document.querySelector('.file__link');
const fileOpenLink = document.querySelector('.file__open-link');
const fileDeleteButton = document.querySelector('.file__delete-button');
const saveNoteButton = document.querySelector('.save');
const noteTagInput = document.querySelector('#tag');
const shareCheckbox = document.querySelector('.share__input input[type="checkbox"]');
const shareEmailInput = document.querySelector('.input__email');
const shareStatusSelect = document.querySelector('.share__options');

// Функция отправки данных на бэкенд
async function sendNoteData() {
   const formData = new FormData();

   // Добавление данных заметки
   formData.append('title', noteTitleInput.value);
   formData.append('text', noteTextInput.value);
   formData.append('tag', noteTagInput.value);

   // Добавление файла (если он загружен)
   if (noteFileInput.files.length > 0) {
       formData.append('file', noteFileInput.files[0]);
   }

   // Добавление данных о доступе
   formData.append('isPublic', shareCheckbox.checked);
   if (shareEmailInput.value) {
       formData.append('sharedWith', JSON.stringify([{
           email: shareEmailInput.value,
           access: shareStatusSelect.value
       }]));
   }

   try {
       const response = await fetch(API_NOTE_CREATE_URL, {
           method: 'POST',
           body: formData
       });

       if (response.ok) {
           const result = await response.json();
           alert('Заметка успешно сохранена!');
           console.log(result);
       } else {
           const error = await response.json();
           alert(`Ошибка сохранения: ${error.message}`);
       }
   } catch (err) {
       console.error('Ошибка отправки данных:', err);
       alert('Не удалось отправить данные на сервер.');
   }
}

// Привязываем обработчик к кнопке сохранения
saveNoteButton.addEventListener('click', (event) => {
   event.preventDefault(); // Предотвращение стандартного поведения формы
   if (!saveNoteButton.classList.contains('isDisabled')) {
       sendNoteData();
   } else {
       alert('Заполните все поля перед сохранением.');
   }
});

// Функция для сохранения данных в localStorage
function saveNoteData() {
   const noteData = {
       title: noteTitleInput.value,
       text: noteTextInput.value,
       tag: noteTagInput.value 
   };
   localStorage.setItem('noteData', JSON.stringify(noteData));
}
// Функция для загрузки данных из localStorage
function loadNoteData() {
  const savedNoteData = localStorage.getItem('noteData');
  if (savedNoteData) {
     const data = JSON.parse(savedNoteData);
     noteTitleInput.value = data.title;
     noteTextInput.value = data.text;
     noteTagInput.value = data.tag;
  }
  toggleSaveButton(); 
}

//Ограничение количества символов в заголовке
if (noteTitleInput) {
   noteTitleInput.addEventListener('input', () => {
      if (noteTitleInput.value.length > MAX_TITLE_LENGTH) {
         noteTitleInput.value = noteTitleInput.value.slice(0, MAX_TITLE_LENGTH);
      }
      saveNoteData();
   });
}

if (noteTextInput) {
   noteTextInput.addEventListener('input', () => {
       saveNoteData(); // Сохраняем данные после изменения текста
   });
}
if (noteTagInput) {
   noteTagInput.addEventListener('input', () => {
       saveNoteData(); // Сохраняем данные после изменения текста
   });
}

//Загрузка файла в заметку и возможность его открыть и просмотреть
noteFileInput.addEventListener('change', () => {
   const file = noteFileInput.files[0];

   const displayFileName = file.name.length > 15 ? file.name.slice(0, 15) + '...' : file.name;

   if (file) {
      const fileURL = URL.createObjectURL(file);
      fileOpenLink.href = fileURL;
      fileOpenLink.textContent = `${displayFileName}`;
      fileLinkContainer.classList.remove('hidden');
   }
});

fileDeleteButton.addEventListener('click', () => {
   noteFileInput.value = '';
   fileOpenLink.href = '#';
   fileOpenLink.textContent = '';
   fileLinkContainer.classList.add('hidden');
});

//Если заголовок и текст заметки пусты, кнопка сохранить недоступна
if (noteTitleInput && noteTextInput && noteTagInput) {
   noteTitleInput.addEventListener('input', (toggleSaveButton));
   noteTextInput.addEventListener('input', toggleSaveButton);
   noteTagInput.addEventListener('input', toggleSaveButton);
}

function toggleSaveButton() {
    if (noteTitleInput.value.length > 0 && noteTextInput.value.length > 0 && noteTagInput.value.length > 0) {
        saveNoteButton.classList.remove('isDisabled');
    } else {
        saveNoteButton.classList.add('isDisabled');
    }
}

loadNoteData();

