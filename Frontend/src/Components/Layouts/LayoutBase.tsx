import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import RegisterPopup from '../RegisterPopup';
import LoginPopup from '../LoginPopup';
import { inject, observer } from 'mobx-react';
import { AppStore } from '../../AppStore';
interface ParentCompProps {
    renderBody: React.ReactNode
    appStore?: AppStore
  }

const LayoutBase: React.FC<ParentCompProps> = (props) => {
    const [showRegisterPopup, setRegisterShowPopup] = useState(false);
    const openRegisterPopup = () => {
        setRegisterShowPopup(true);
    }
    
    const closeRegisterPopup = () => {
        setRegisterShowPopup(false);
    }
    const [showLoginPopup, setLoginShowPopup] = useState(false);
    const openLoginPopup = () => {
        setLoginShowPopup(true);
    }
    
    const closeLoginPopup = () => {
        setLoginShowPopup(false);
    }
    return (
        <div className='layout-base'>
            {showRegisterPopup && <RegisterPopup onClose={closeRegisterPopup} />}
            {showLoginPopup && <LoginPopup onClose={closeLoginPopup} />}
            <table className='container-table'>
                <tr className='bar-cell'>
                    <div className='bar-element'>
                        <table>
                            <tr>
                            <td className='theatre-title'><Link to="/"><h1>Театральная студия "В Созвездиях"</h1></Link></td>
                            {!props.appStore?.authInfo.IsAuthorize ?(
                            <td className='auth'>
                                <table className='auth-table'>
                                    <tr>
                                        <td><button className='button' onClick={openLoginPopup}>Войти</button></td>
                                        <td><button className='button' onClick={openRegisterPopup}>Регистрация</button></td>
                                    </tr>
                                </table>
                            </td>
                            ):null}
                            </tr>
                        </table>
                    </div>
                </tr>    
                <tr className="top-menu-cell">
                    <div className='top-menu-element'>
                        <div className='background'>
                        <table className='link-table'>
                            <tr>
                            <td><Link to="/Gallery">Галерея</Link></td>
                            <td><Link to="/Shows">Спектакли</Link></td>
                            <td><Link to="/Contacts">Контакты</Link></td>
                            <td><Link to="/Feedback">Обратная связь</Link></td>
                            </tr>
                        </table>
                        </div>
                    </div>
                </tr>
                <tr className="main-content-cell">
                    <div className='main-content-element'>
                        {props.renderBody}
                    </div>
                </tr>
                <tr className='footer-cell'>
                    <footer>
                        <p>&copy; 2023 Моя Компания</p>
                        <p>Контактная информация: example@example.com</p>
                    </footer>
                </tr>
            </table>
        </div>
    );
}

export default inject('appStore')(observer(LayoutBase))