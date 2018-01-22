import { Injectable, Injector } from '@angular/core';
import { HttpHandler, HttpEvent, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private injector: Injector) { }
    intercept(
        request: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {
        // tslint:disable-next-line:prefer-const
        let auth = this.injector.get(AuthService);
        // tslint:disable-next-line:no-non-null-assertion
        const token = (auth.isLoggedIn()) ? auth.getAuth()!.token : null;
        if (token) {
            request = request.clone({
                setHeaders: {
                            Authorization: `Bearer ${token}`
                        }
        }); }
        return next.handle(request);
    }
}
