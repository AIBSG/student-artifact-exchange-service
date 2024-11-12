const USER_STATUS = {
   READING: 'Чтение',
   EDITING: 'Редактирование'
}

const MAX_TITLE_LENGTH = 25;
const MAX_TEXT_LENGTH = 250;

const MAX_FILE_TITLE_LENGTH = 25;
const MAX_FILE_TEXT_LENGTH = 50;

const MAIL_REGEX = /^[a-zA-Z][a-zA-Z0-9\-\_\.]+@[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}$/;

export { USER_STATUS, 
         MAX_TITLE_LENGTH, 
         MAX_TEXT_LENGTH,
         MAX_FILE_TITLE_LENGTH, 
         MAX_FILE_TEXT_LENGTH,
         MAIL_REGEX,
      };