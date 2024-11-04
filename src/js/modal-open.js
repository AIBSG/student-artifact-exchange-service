const shareButton = document.querySelector('.note__share-button');
const shareOpen = document.querySelector('.share__open');
const cancelButton = document.querySelector('.share__cancel');
const deleteNoteButton = document.querySelector('.delete');
const approveOpen = document.querySelector('.approve__open');
const cancelDeleteButton = document.querySelector('.approve__cancel');
const inputEmail = document.querySelector('.input__email');

//Открытие и закрытие окна с возможностью поделиться заметкой
function openShareModal() {
   shareButton.addEventListener('click', () => {
      shareOpen.classList.remove('hidden');
   });
}
openShareModal();

function closeShareModal() {
   cancelButton.addEventListener('click', () => {
      shareOpen.classList.add('hidden');
   });
}
closeShareModal();


//Окрытие и закрытие окна с подтверждением удаления заметки
function openApproveModal() {
   deleteNoteButton.addEventListener('click', () => {
      approveOpen.classList.remove('hidden');
   });
}
openApproveModal();

function closeApproveModal() {
   cancelDeleteButton.addEventListener('click', () => {
      approveOpen.classList.add('hidden');
   });
}
closeApproveModal();
