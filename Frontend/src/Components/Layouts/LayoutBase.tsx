import React from 'react';
interface ParentCompProps {
    renderBody: React.ReactNode;
    isAuthorize: boolean
  }

const LayoutBase: React.FC<ParentCompProps> = (props) => {
    return (
        <div className='layout-base'>
            <table className='container-table'>
                <tr className='bar-cell'>
                    <div className='bar-element'>
                        <table>
                            <tr>
                            <td><h1>Театральная студия "В Созвездиях"</h1></td>
                            {!props.isAuthorize ?(
                            <td className='auth'>
                                <table className='auth-table'>
                                    <tr>
                                        <td><a href=''>Войти</a></td>
                                        <td><a href=''>Регистрация</a></td>
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
                            <td><a href="">Галерея</a></td>
                            <td><a href="">Спектакли</a></td>
                            <td><a href="">Контакты</a></td>
                            <td><a href="">О нас</a></td>
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

export default LayoutBase