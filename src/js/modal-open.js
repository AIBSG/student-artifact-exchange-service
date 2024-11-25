import { MAIL_REGEX } from "./const.js";
import { API_SHARE_URL } from "./api.js";

const shareButton = document.querySelector('.note__share-button');
const shareOpen = document.querySelector('.share__open');
const cancelButton = document.querySelector('.share__cancel');
const deleteNoteButton = document.querySelector('.delete');
const approveOpen = document.querySelector('.approve__open');
const cancelDeleteButton = document.querySelector('.approve__cancel');
const inputEmail = document.querySelector('.input__email');
const shareOptions = document.querySelector('.share__options');
const saveShareButton = document.querySelector('.share__save');
const readForAllCheckbox = document.querySelector('.input__checkbox');

// Открытие и закрытие окна с возможностью поделиться заметкой
if (shareButton) {
   shareButton.addEventListener('click', openShareModal);
}

if (cancelButton) {
   cancelButton.addEventListener('click', closeShareModal);
}

// Включение/отключение кнопки "Сохранить" в зависимости от условий
function toggleSaveButtonState() {
   const isEmailValid = inputEmail.value.trim() !== '';
   const isReadForAllChecked = readForAllCheckbox && readForAllCheckbox.checked;

   saveShareButton.disabled = !(isEmailValid || isReadForAllChecked);
   saveShareButton.classList.toggle('disabled', saveShareButton.disabled);
}

if (inputEmail) {
   inputEmail.addEventListener('input', toggleSaveButtonState);
}
if (readForAllCheckbox) {
   readForAllCheckbox.addEventListener('change', toggleSaveButtonState);
}

// Сохранение и отображение введенных данных в окне "Поделиться"
saveShareButton.addEventListener('click', saveShareModal);

async function saveShareModal() {
   const emailShareInput = inputEmail.value.trim();
   const accessMode = shareOptions.value;
   const isReadForAllChecked = readForAllCheckbox && readForAllCheckbox.checked;

   if (!emailShareInput && !isReadForAllChecked) {
      alert('Введите email или выберите "Чтение для всех".');
      return;
   }

   if (emailShareInput && !MAIL_REGEX.test(emailShareInput)) {
      alert('Введите корректный email.');
      return;
   }

   // Подготовка данных для отправки
   const data = {
      email: emailShareInput || null,
      accessMode: accessMode || null,
      readForAll: isReadForAllChecked,
   };

   try {
      const response = await fetch(API_SHARE_URL, {
         method: 'POST',
         headers: {
            'Content-Type': 'application/json',
         },
         body: JSON.stringify(data),
      });

      if (!response.ok) {
         throw new Error(`Ошибка: ${response.statusText}`);
      }

      const result = await response.json();

      // Уведомление об успешной отправке
      alert(result.message || 'Данные успешно сохранены.');
      closeShareModal(); // Закрываем окно после успешного сохранения

   } catch (error) {
      console.error('Ошибка при отправке данных:', error);
      alert('Не удалось сохранить данные. Попробуйте снова.');
   }
}

// Открытие и закрытие окна с подтверждением удаления заметки
if (deleteNoteButton) {
   deleteNoteButton.addEventListener('click', () => {
      approveOpen.classList.remove('hidden');
   });
}

if (cancelDeleteButton) {
   cancelDeleteButton.addEventListener('click', () => {
      approveOpen.classList.add('hidden');
   });
}

function openShareModal() {
   if (shareOpen) {
      shareOpen.classList.remove('hidden');
   }
   toggleSaveButtonState();
}

function closeShareModal() {
   shareOpen.classList.add('hidden');
   inputEmail.value = '';
   shareOptions.selectedIndex = 0;
   if (readForAllCheckbox) {
      readForAllCheckbox.checked = false;
   }
   toggleSaveButtonState();
}

export { openShareModal, closeShareModal, saveShareModal };
