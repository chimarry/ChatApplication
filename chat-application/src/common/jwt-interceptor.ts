import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        let authenticateCredentials = JSON.parse(localStorage.getItem('authenticateCredentials'));
        if (authenticateCredentials && authenticateCredentials.accessToken) {
            request = request.clone({
                setHeaders: { 
                    Authorization: `Bearer ${authenticateCredentials.accessToken}`,
                    'x-api-key':'2c4eed8d-3454-47b8-b555-2726d44012bf'
                }
            });
        } else {
            request = request.clone({
                setHeaders: { 
                    'x-api-key':'2c4eed8d-3454-47b8-b555-2726d44012bf'
                }
            });
        }

        return next.handle(request);
    }
}