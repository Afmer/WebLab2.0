import { action, makeAutoObservable, reaction } from 'mobx';
import AuthInfo from './Interfaces/AuthInfo';
import ShortShow from './Interfaces/ShortShow';
import axios from 'axios';

export class AppStore {
  authInfo: AuthInfo | null = null;
  favoriteShows: ShortShow[] | null = null;

  constructor() {
    makeAutoObservable(this, {
      updateAuth: action,
      addFavoriteShow: action,
      removeFavoriteShow: action
    });
    reaction(() => this.authInfo,
    async (newAuthInfo, oldAuthInfo) => {
      if(newAuthInfo?.IsAuthorize)
      {
        this.getFavorite()
          .then(data => {
            this.favoriteShows = data
          })
          .catch(error => {
            console.error('Ошибка при получении данных:', error);
          });
      }
    })
  }
  private async getFavorite(): Promise<ShortShow[]>
  {
    try
    {
      const response = await axios.get('api/FavoriteShow')
      return response.data as ShortShow[]
    }
    catch (error) {
      console.error('Произошла ошибка:', error);
      throw error;
    }
  }
  updateAuth(newData: AuthInfo) {
    this.authInfo = newData;
  }
  addFavoriteShow(show: ShortShow)
  {
    this.favoriteShows?.push(show)
  }
  removeFavoriteShow(show: ShortShow)
  {
    if(this.favoriteShows !== null)
      this.favoriteShows = this.favoriteShows.filter(x => x.id !== show.id)
  }
}

export const appStore = new AppStore();