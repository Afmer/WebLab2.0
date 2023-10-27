import React, { useState, ChangeEvent, FormEvent } from 'react';
import '../CSS/Popup.css'
interface LoginPopupProps {
  onClose: () => void;
}

interface FormData {
  username: string;
  password: string;
  confirmPassword: string;
}

const LoginPopup: React.FC<LoginPopupProps> = ({ onClose }) => {
  const [formData, setFormData] = useState<FormData>({
    username: '',
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

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();

    onClose();
  };

  return (
    <div className="popup">
      <div className="content">
        <span className="close-btn" onClick={onClose}>&times;</span>
        <h2 className='label'>Вход</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="username">Имя пользователя:</label>
            <input
              type="text"
              id="username"
              name="username"
              value={formData.username}
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

export default LoginPopup;