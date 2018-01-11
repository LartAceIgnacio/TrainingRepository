import { Injectable, Inject, EventEmitter, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser  } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import 'rxjs/Rx';
import { CLIENT_RENEG_WINDOW } from 'tls';

import { API_URL} from './constants';

@Injectable()
export class AuthService {
  authKey: string = "auth";
  clientId: string = "DigiBookWeb";
  baseUrl: string = `${API_URL}/`;
  
  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: any) {
    
  }

  login(username: string, password: string): Observable<boolean> {
    var url =this.baseUrl + "token/auth";

    var data = {
      username: username,
      password: password,
      client_id: this.clientId,
      grant_type: "password",
      scope: "offline_access profile email"
    };

    return this.http.post<TokenResponse>(url, data)
      .map((res) => {
        let token = res && res.token;
        if (token) {
          this.setAuth(res);
          return true;
        }
        return Observable.throw('Unauthorized');
      }).catch(error => {
        return new Observable<any>(error);
      });
  }

  logout(): boolean {
    this.setAuth(null);
    return true;
  }

  setAuth(auth: TokenResponse | null) : boolean {
    if (isPlatformBrowser(this.platformId)) {
      if (auth) {
        localStorage.setItem( this.authKey, JSON.stringify(auth));
      } else {
        localStorage.removeItem(this.authKey);
      }
    }
    return true;
  }

  getAuth(): TokenResponse | null {
    if (isPlatformBrowser(this.platformId)) {
      var item = localStorage.getItem(this.authKey);
      if (item) {
        return JSON.parse(item);
      }
      
    }
    return null;
  }

  isLoggedIn() : boolean {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.authKey) != null;
    }
    return false;
  }
}
