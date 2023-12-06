import '../CSS/Popup.css'
import React, { ChangeEvent, useState } from 'react';
interface FeedbackFormPopupProps {
    onClose: () => void;
  }

interface FormData {
  message: string;
}
const FeedbackForm: React.FC<FeedbackFormPopupProps> = ({onClose}) => {
  const [formData, setFormData] = useState<FormData>({
    message: ''
  });
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Добавьте здесь логику обработки отправки формы, например, отправку данных на сервер
  };
  const handleChange = (e: ChangeEvent<HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    })
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
            <textarea rows={4} value={formData.message} onChange={handleChange} id="message" name="message" required></textarea>
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
