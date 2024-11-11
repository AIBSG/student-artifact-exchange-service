const shareButton = document.querySelector('.note__share-button');
const shareOpen = document.querySelector('.share__open');
const cancelButton = document.querySelector('.share__cancel');
const deleteNoteButton = document.querySelector('.delete');
const approveOpen = document.querySelector('.approve__open');
const cancelDeleteButton = document.querySelector('.approve__cancel');
const inputEmail = document.querySelector('.input__email');
const shareOptions = document.querySelector('.share__options');

//Открытие и закрытие окна с возможностью поделиться заметкой
if (shareButton) {
   shareButton.addEventListener('click', openShareModal)
}

if (cancelButton) {
   cancelButton.addEventListener('click', closeShareModal)
}

//Окрытие и закрытие окна с подтверждением удаления заметки
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
}

function closeShareModal() {
   shareOpen.classList.add('hidden');
   inputEmail.value = '';
   shareOptions.selectedIndex = 0;
}

export {openShareModal, closeShareModal};