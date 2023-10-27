import React, { useState, ChangeEvent, FormEvent } from 'react';
import '../CSS/Popup.css'
import axios from 'axios';
interface RegisterPopupProps {
  onClose: () => void;
}

interface FormData {
  login: string;
  password: string;
  confirmPassword: string;
  email: string;
}

const RegisterPopup: React.FC<RegisterPopupProps> = ({ onClose }) => {
  const [formData, setFormData] = useState<FormData>({
    login: '',
    password: '',
    confirmPassword: '',
    email: ''
  });

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    e.persist();
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/Identity/Register', formData);

      if (response.status === 200) {
        console.log('Регистрация успешно завершена!');
        onClose();
      }
    } catch (error) {
      console.error('Ошибка:', error);
    }
  };

  return (
    <div className="popup">
      <div className="content">
        <span className="close-btn" onClick={onClose}>&times;</span>
        <h2 className='label'>Регистрация</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="username">Имя пользователя:</label>
            <input
              type="username"
              id="username"
              name="login"
              value={formData.login}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Пароль:</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="confirmPassword">Подтверждение пароля:</label>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
            />
            <div className="form-group">
              <label htmlFor="email">Email:</label>
              <input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                required
              />
            </div>
          </div>
          <button type="submit" className="submit-button">Зарегистрироваться</button>
        </form>
      </div>
    </div>
  );
};

export default RegisterPopup;
