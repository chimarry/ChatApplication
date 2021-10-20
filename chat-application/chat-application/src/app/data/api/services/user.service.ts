/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders } from '@angular/common/http';
import { BaseService as __BaseService } from '../base-service';
import { ApiConfiguration as __Configuration } from '../api-configuration';
import { StrictHttpResponse as __StrictHttpResponse } from '../strict-http-response';
import { Observable as __Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

import { RegisterUserResponseWrapper } from '../models/register-user-response-wrapper';
import { RegisterUserPostWrapper } from '../models/register-user-post-wrapper';
import { AuthenticateCredentialsResponseWrapper } from '../models/authenticate-credentials-response-wrapper';
import { AuthenticateResponseWrapper } from '../models/authenticate-response-wrapper';
import { AuthenticateOtpPostWrapper } from '../models/authenticate-otp-post-wrapper';
@Injectable({
  providedIn: 'root',
})
class UserService extends __BaseService {
  static readonly postApiV01UsersPath = '/api/v0.1/users';
  static readonly postApiV01UsersAuthenticateLoginFormPath = '/api/v0.1/users/authenticate/login-form';
  static readonly postApiV01UsersAuthenticateOtpOtpApiKeyPath = '/api/v0.1/users/authenticate/otp/{otpApiKey}';
  static readonly postApiV01UsersIsLoggedInPath = '/api/v0.1/users/is-logged-in';
  static readonly postApiV01UsersRefreshTokenPath = '/api/v0.1/users/refresh-token';
  static readonly postApiV01UsersRevokeTokenPath = '/api/v0.1/users/revoke-token';

  constructor(
    config: __Configuration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Enables user to register. Certificate is generated and sent on user's email.
   * @param body Information neccessary for registration process
   * @return Success
   */
  postApiV01UsersResponse(body?: RegisterUserPostWrapper): __Observable<__StrictHttpResponse<RegisterUserResponseWrapper>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = body;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/users`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<RegisterUserResponseWrapper>;
      })
    );
  }
  /**
   * Enables user to register. Certificate is generated and sent on user's email.
   * @param body Information neccessary for registration process
   * @return Success
   */
  postApiV01Users(body?: RegisterUserPostWrapper): __Observable<RegisterUserResponseWrapper> {
    return this.postApiV01UsersResponse(body).pipe(
      __map(_r => _r.body as RegisterUserResponseWrapper)
    );
  }

  /**
   * Enables user to authenticate using credentials and certificate.
   * This authentication is limited, it can be used as one phase of
   * MF authentication, because it grants access only to another
   * authentication endpoint. OTP code is generated and sent on user's email,
   * as well as OTP api key (returned in request).
   * @param params The `UserService.PostApiV01UsersAuthenticateLoginFormParams` containing the following parameters:
   *
   * - `Username`: Username
   *
   * - `Password`: Password
   *
   * - `Certificate`: Certificate
   *
   * @return Success
   */
  postApiV01UsersAuthenticateLoginFormResponse(params: UserService.PostApiV01UsersAuthenticateLoginFormParams): __Observable<__StrictHttpResponse<AuthenticateCredentialsResponseWrapper>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let __formData = new FormData();
    __body = __formData;
    if (params.Username != null) { __formData.append('Username', params.Username as string | Blob);}
    if (params.Password != null) { __formData.append('Password', params.Password as string | Blob);}
    if (params.Certificate != null) { __formData.append('Certificate', params.Certificate as string | Blob);}
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/users/authenticate/login-form`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<AuthenticateCredentialsResponseWrapper>;
      })
    );
  }
  /**
   * Enables user to authenticate using credentials and certificate.
   * This authentication is limited, it can be used as one phase of
   * MF authentication, because it grants access only to another
   * authentication endpoint. OTP code is generated and sent on user's email,
   * as well as OTP api key (returned in request).
   * @param params The `UserService.PostApiV01UsersAuthenticateLoginFormParams` containing the following parameters:
   *
   * - `Username`: Username
   *
   * - `Password`: Password
   *
   * - `Certificate`: Certificate
   *
   * @return Success
   */
  postApiV01UsersAuthenticateLoginForm(params: UserService.PostApiV01UsersAuthenticateLoginFormParams): __Observable<AuthenticateCredentialsResponseWrapper> {
    return this.postApiV01UsersAuthenticateLoginFormResponse(params).pipe(
      __map(_r => _r.body as AuthenticateCredentialsResponseWrapper)
    );
  }

  /**
   * User must be authenticated to access this endpoint using specific OTP api key.
   * This endpoint can be used as second phase of MF authentication.
   * If authentication with OTP is sucessfull, JWT access token is returned in body,
   * and refresh token is returned as cookie. User can use JWT access token limited number
   * of minutes, so it is best to refresh token or to login again.
   * @param params The `UserService.PostApiV01UsersAuthenticateOtpOtpApiKeyParams` containing the following parameters:
   *
   * - `otpApiKey`: Unique OTP api key
   *
   * - `body`: Information regarding OTP code
   *
   * @return Success
   */
  postApiV01UsersAuthenticateOtpOtpApiKeyResponse(params: UserService.PostApiV01UsersAuthenticateOtpOtpApiKeyParams): __Observable<__StrictHttpResponse<AuthenticateResponseWrapper>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    __body = params.body;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/users/authenticate/otp/${encodeURIComponent(String(params.otpApiKey))}`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<AuthenticateResponseWrapper>;
      })
    );
  }
  /**
   * User must be authenticated to access this endpoint using specific OTP api key.
   * This endpoint can be used as second phase of MF authentication.
   * If authentication with OTP is sucessfull, JWT access token is returned in body,
   * and refresh token is returned as cookie. User can use JWT access token limited number
   * of minutes, so it is best to refresh token or to login again.
   * @param params The `UserService.PostApiV01UsersAuthenticateOtpOtpApiKeyParams` containing the following parameters:
   *
   * - `otpApiKey`: Unique OTP api key
   *
   * - `body`: Information regarding OTP code
   *
   * @return Success
   */
  postApiV01UsersAuthenticateOtpOtpApiKey(params: UserService.PostApiV01UsersAuthenticateOtpOtpApiKeyParams): __Observable<AuthenticateResponseWrapper> {
    return this.postApiV01UsersAuthenticateOtpOtpApiKeyResponse(params).pipe(
      __map(_r => _r.body as AuthenticateResponseWrapper)
    );
  }

  /**
   * @return Success
   */
  postApiV01UsersIsLoggedInResponse(): __Observable<__StrictHttpResponse<boolean>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/users/is-logged-in`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'text'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return (_r as HttpResponse<any>).clone({ body: (_r as HttpResponse<any>).body === 'true' }) as __StrictHttpResponse<boolean>
      })
    );
  }
  /**
   * @return Success
   */
  postApiV01UsersIsLoggedIn(): __Observable<boolean> {
    return this.postApiV01UsersIsLoggedInResponse().pipe(
      __map(_r => _r.body as boolean)
    );
  }

  /**
   * Refreshes token (only once per token). New JWT token is generated,
   * as well as new refresh token (Rotation of Refresh Token).
   * @return Success
   */
  postApiV01UsersRefreshTokenResponse(): __Observable<__StrictHttpResponse<AuthenticateResponseWrapper>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/users/refresh-token`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<AuthenticateResponseWrapper>;
      })
    );
  }
  /**
   * Refreshes token (only once per token). New JWT token is generated,
   * as well as new refresh token (Rotation of Refresh Token).
   * @return Success
   */
  postApiV01UsersRefreshToken(): __Observable<AuthenticateResponseWrapper> {
    return this.postApiV01UsersRefreshTokenResponse().pipe(
      __map(_r => _r.body as AuthenticateResponseWrapper)
    );
  }

  /**
   * Revokes current JWT token.
   * @return Success
   */
  postApiV01UsersRevokeTokenResponse(): __Observable<__StrictHttpResponse<boolean>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/users/revoke-token`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'text'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return (_r as HttpResponse<any>).clone({ body: (_r as HttpResponse<any>).body === 'true' }) as __StrictHttpResponse<boolean>
      })
    );
  }
  /**
   * Revokes current JWT token.
   * @return Success
   */
  postApiV01UsersRevokeToken(): __Observable<boolean> {
    return this.postApiV01UsersRevokeTokenResponse().pipe(
      __map(_r => _r.body as boolean)
    );
  }
}

module UserService {

  /**
   * Parameters for postApiV01UsersAuthenticateLoginForm
   */
  export interface PostApiV01UsersAuthenticateLoginFormParams {

    /**
     * Username
     */
    Username: string;

    /**
     * Password
     */
    Password: string;

    /**
     * Certificate
     */
    Certificate: File;
  }

  /**
   * Parameters for postApiV01UsersAuthenticateOtpOtpApiKey
   */
  export interface PostApiV01UsersAuthenticateOtpOtpApiKeyParams {

    /**
     * Unique OTP api key
     */
    otpApiKey: string;

    /**
     * Information regarding OTP code
     */
    body?: AuthenticateOtpPostWrapper;
  }
}

export { UserService }
