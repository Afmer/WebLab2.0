import axios from 'axios';
import '../CSS/Popup.css'
import React, { ChangeEvent, useState } from 'react';
interface FeedbackFormPopupProps {
    onClose: () => void;
  }

interface FormData {
  text: string;
  label: string;
}
const FeedbackForm: React.FC<FeedbackFormPopupProps> = ({onClose}) => {
  const [formData, setFormData] = useState<FormData>({
    text: '',
    label: ''
  });
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/Feedback', formData);

      if (response.status === 200) {
        console.log('Форма отправлена');
        onClose();
      }
    } catch (error) {
      console.error('Ошибка:', error);
    }
  };
  const handleChange = (e: ChangeEvent<HTMLTextAreaElement> | ChangeEvent<HTMLInputElement>) => {
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
            <label htmlFor="label" className="label">
              Заголовок:
            </label>
            <input id='label' name='label' value={formData.label} onChange={handleChange} type='text' maxLength={50}/>
            <label htmlFor="text" className="label">
              Сообщение:
            </label>
            <textarea rows={4} value={formData.text} onChange={handleChange} id="text" name="text" required></textarea>
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
