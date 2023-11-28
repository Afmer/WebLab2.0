import React, { useState, ChangeEvent, FormEvent } from 'react';
import '../CSS/Popup.css'
import { AppStore } from '../AppStore';
import { inject, observer } from 'mobx-react';
import axios from 'axios';
import InitIdentity from './Functions/InitIdentity';
interface LoginPopupProps {
  onClose: () => void;
  appStore?: AppStore
}

interface FormData {
  login: string;
  password: string;
  confirmPassword: string;
}

const LoginPopup: React.FC<LoginPopupProps> = ({ onClose, appStore }) => {
  const [formData, setFormData] = useState<FormData>({
    login: '',
    password: '',
    confirmPassword: '',
  });

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/Identity/Login', formData);

      if (response.status === 200) {
        console.log('Регистрация успешно завершена!');
        InitIdentity(appStore!);
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
        <h2 className='label'>Вход</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="login">Имя пользователя:</label>
            <input
              type="text"
              id="login"
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
          <button type="submit" className="submit-button">Войти</button>
        </form>
      </div>
    </div>
  );
};

export default inject('appStore')(observer(LoginPopup));