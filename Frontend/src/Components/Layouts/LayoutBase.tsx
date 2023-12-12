import React, { useEffect, useState } from 'react';
import { Link, NavLink } from 'react-router-dom';
import RegisterPopup from '../RegisterPopup';
import LoginPopup from '../LoginPopup';
import { inject, observer } from 'mobx-react';
import { AppStore } from '../../AppStore';
import axios from 'axios';
import IdentityBase from '../../Interfaces/IdentityBase';
import InitIdentity from '../Functions/InitIdentity';
import FeedbackForm from '../FeedbackForm';
import SearchBar from '../SearchBar';
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
    const [showFeedbackFormPopup, setFeedbackFormPopup] = useState(false);
    const openFeedbackFormPopup = () => {
        setFeedbackFormPopup(true)
    }
    const closeFeedbackFormPopup = () => {
        setFeedbackFormPopup(false)
    }
    const logoutEvent = async () => {
        await axios.get('/api/Identity/Logout')
        props.appStore?.updateAuth({IsAuthorize: false, IsAdmin: false, Username: ""})
    }
    useEffect(() => {InitIdentity(props.appStore!)}, [])
    return (
        <div className='layout-base'>
            {showRegisterPopup && <RegisterPopup onClose={closeRegisterPopup} />}
            {showLoginPopup && <LoginPopup onClose={closeLoginPopup} />}
            {showFeedbackFormPopup && <FeedbackForm onClose={closeFeedbackFormPopup}/>}
            <table className='container-table'>
                <tr className='bar-cell'>
                    <div className='bar-element'>
                        <table>
                            <tr>
                            <td className='theatre-title'><Link to="/"><h1>Театральная студия "В Созвездиях"</h1></Link></td>
                            <td className='search-bar-cell'><SearchBar/></td>
                            <td className='auth'>
                                <table className='auth-table'>
                                    <tr>
                                        {props.appStore?.authInfo !== null ?(
                                            !props.appStore?.authInfo.IsAuthorize ?(
                                                <>
                                                    <td><button className='button' onClick={openLoginPopup}>Войти</button></td>
                                                    <td><button className='button' onClick={openRegisterPopup}>Регистрация</button></td>
                                                </>
                                            ): (
                                                <>
                                                    <td>{props.appStore?.authInfo.Username}</td>
                                                    <td><button className='button' onClick={logoutEvent}>Выйти</button></td>
                                                </>
                                        )) : <p>wait</p>}
                                    </tr>
                                    <tr>
                                            {props.appStore?.authInfo !== null ?(
                                                props.appStore?.authInfo.IsAuthorize ?(
                                                    <>
                                                        <td>
                                                            <table className='mini-button-group'>
                                                            <tr>
                                                                <td></td>
                                                                <td>{props.appStore?.authInfo.IsAdmin ?(
                                                                <Link to="/AddShow"><button className='add-show'></button></Link>
                                                                ): null}</td>
                                                                <td>
                                                                    <Link to="/Favorites">
                                                                        <button className='favorite'></button>
                                                                    </Link>
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <button className='button' onClick={openFeedbackFormPopup}>Обратная связь</button>
                                                        </td>
                                                    </>
                                                ): (
                                                    <>
                                                    </>
                                            )) : <p>wait</p>}
                                    </tr>
                                </table>
                            </td>
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
                            <td><Link to="/News">Новости</Link></td>
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