import '../CSS/Popup.css'
import React from 'react';
interface FeedbackFormPopupProps {
    onClose: () => void;
  }
const FeedbackForm: React.FC<FeedbackFormPopupProps> = ({onClose}) => {
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Добавьте здесь логику обработки отправки формы, например, отправку данных на сервер
  };

  return (
    <div className="popup">
      <div className="content">
        <span className="close-btn" onClick={onClose}>&times;</span>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="message" className="label">
              Сообщение:
            </label>
            <textarea id="message" name="message" rows={4} required></textarea>
          </div>
          <button type="submit" className="submit-button">
            Отправить
          </button>
        </form>
      </div>
    </div>
  );
};

export default FeedbackForm;
