document.addEventListener('DOMContentLoaded', () => {
    // Переход для кнопки "Войти".
    const loginBtn = document.querySelector('.login-btn');
    if (loginBtn) {
        loginBtn.addEventListener('click', () => {
            window.location.href = 'login-form.html';
        });
    }

    // Переход для кнопки "Зарегистрироваться"
    const registerBtn = document.querySelector('.register-btn');
    if (registerBtn) {
        registerBtn.addEventListener('click', () => {
            window.location.href = 'registration-form.html';
        });
    }

    // Переход для кнопки "Смотреть все (Мои заметки)"
    const allCheckMyNotesBtn = document.querySelector('.allcheck-my-notes__btn');
    if (allCheckMyNotesBtn) {
        allCheckMyNotesBtn.addEventListener('click', () => {
            window.location.href = 'my-note-form.html'; 
        });
    }

    // Переход для кнопки "Смотреть все (Мои файлы)"
    const allCheckMyFilesBtn = document.querySelector('.allcheck-my-files__btn');
    if (allCheckMyFilesBtn) {
        allCheckMyFilesBtn.addEventListener('click', () => {
            window.location.href = 'my-file-form.html';
        });
    }

  // Переход для кнопки "Главная"
  const mainLink = document.querySelector('.sidebar__main');
  if (mainLink) {
      mainLink.addEventListener('click', (e) => {
          e.preventDefault(); 
          window.location.href = 'note-form.html'; 
      });
  }

  // Переход для кнопки "Новая заметка"
  const newNoteLink = document.querySelector('.sidebar__btn-note');
  if (newNoteLink) {
      newNoteLink.addEventListener('click', () => {
          window.location.href = 'new-note.html';
      });
  }

});