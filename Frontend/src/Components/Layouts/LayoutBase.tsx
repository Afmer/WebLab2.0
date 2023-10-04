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
                            <td>
                                <table>
                                    <tr>
                                        <td><input type="text" value="логин"/></td>
                                    </tr>
                                    <tr>
                                        <td><input type="password" value="логин"/></td>
                                    </tr>
                                    <tr>
                                        <td><input type="submit" value="войти"/></td>
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
                        <table>
                            <tr>
                            <td><a href="~/MainChapter/Gallery">Галерея</a></td>
                            <td><a href="~/MainChapter/Vacancies">Вакансии</a></td>
                            <td><a href="~/MainChapter/Contacts">Контакты</a></td>
                            <td><a href="~/MainChapter/About">О нас</a></td>
                            </tr>
                        </table>
                        </div>
                    </div>
                </tr>
                <tr className="main-content-cell">
                    <table>
                        <tr>
                            <td className="sidebar">
                                <table>
                                    <tr>
                                        <td><a href="~/SubChapter/Courses" className="element">Кружки</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="~/SubChapter/FastCourses" className="element">Интенсивные курсы</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="~/SubChapter/Services" className="element">Услуги</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="~/SubChapter/FAQ" className="element">Частые вопросы</a></td>
                                    </tr>
                                </table>
                            </td>
                            <td className='content'>
                                {props.renderBody}
                            </td>
                        </tr>
                    </table>
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