import { action, makeAutoObservable } from 'mobx';
import AuthInfo from './Interfaces/AuthInfo';

export class AppStore {
  authInfo: AuthInfo = {IsAuthorize: false, IsAdmin: false};

  constructor() {
    makeAutoObservable(this, {
      updateAuth: action
    });
  }

  updateAuth(newData: AuthInfo) {
    this.authInfo = newData;
  }
}

export const appStore = new AppStore();