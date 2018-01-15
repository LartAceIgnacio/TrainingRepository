import { Injectable, Inject, EventEmitter, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
// tslint:disable-next-line:import-blacklist
import { Observable } from 'rxjs';
// tslint:disable-next-line:import-blacklist
import 'rxjs/Rx';
// import { CLIENT_RENEG_WINDOW } from 'tls';

import { API_URL } from './constants';

@Injectable()
export class AuthService {
  // tslint:disable-next-line:no-inferrable-types
  authKey: string = 'auth';
  // tslint:disable-next-line:no-inferrable-types
  clientId: string = 'DayTwoApp';
  // tslint:disable-next-line:no-inferrable-types
  baseUrl: string = `${API_URL}/`;

  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: any) {

  }

  login(username: string, password: string): Observable<boolean> {
    // tslint:disable-next-line:prefer-const
    let url = this.baseUrl + 'token/auth';

    // tslint:disable-next-line:prefer-const
    let data = {
      username: username,
      password: password,
      client_id: this.clientId,
      grant_type: 'password',
      scope: 'offline_access profile email'
    };

    // return this.http.post<TokenResponse>(url, data)
    //   .map((res) => {
    //     // tslint:disable-next-line:prefer-const
    //     let token = res && res.token;
    //     if (token) {
    //       this.setAuth(res);
    //       return true;
    //     }
    //     return Observable.throw('Unauthorized');
    //   }).catch(error => {
    //     return new Observable<any>(error);
    //   });

    return this.getAuthFromServer(url, data);
  }

  logout(): boolean {
    this.setAuth(null);
    return true;
  }

  setAuth(auth: TokenResponse | null): boolean {
    if (isPlatformBrowser(this.platformId)) {
      if (auth) {
        localStorage.setItem(this.authKey, JSON.stringify(auth));
      } else {
        localStorage.removeItem(this.authKey);
      }
    }
    return true;
  }

  getAuth(): TokenResponse | null {
    if (isPlatformBrowser(this.platformId)) {
      // tslint:disable-next-line:prefer-const
      let item = localStorage.getItem(this.authKey);
      if (item) {
        return JSON.parse(item);
      }

    }
    return null;
  }

  isLoggedIn(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.authKey) != null;
    }
    return false;
  }

  // try to refresh token
  refreshToken(): Observable<boolean> {
    const url = 'api/token/auth';
    // tslint:disable-next-line:prefer-const
    let data = {
      client_id: this.clientId,
      // required when signing up with username/password
      grant_type: 'refresh_token',
      // tslint:disable-next-line:no-non-null-assertion
      refresh_token: this.getAuth()!.refresh_token,
      // space-separated list of scopes for which the token is issued
      scope: 'offline_access profile email'
    };
    return this.getAuthFromServer(url, data);
  }

  // retrieve the access & refresh tokens from the server
  getAuthFromServer(url: string, data: any): Observable<boolean> {
    return this.http.post<TokenResponse>(url, data)
      .map((res) => {
        // tslint:disable-next-line:prefer-const
        let token = res && res.token;
        // if the token is there, login has been successful
        if (token) {
          // store username and jwt token
          this.setAuth(res);
          // successful login
          return true;
        }
        // failed login
        console.log('error');
        return Observable.throw('Unauthorized');
      })
      .catch(error => {
        console.log(error);
        return new Observable<any>(error);
      });
  }
}
