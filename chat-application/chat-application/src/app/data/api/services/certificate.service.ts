/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders } from '@angular/common/http';
import { BaseService as __BaseService } from '../base-service';
import { ApiConfiguration as __Configuration } from '../api-configuration';
import { StrictHttpResponse as __StrictHttpResponse } from '../strict-http-response';
import { Observable as __Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
class CertificateService extends __BaseService {
  static readonly getApiV01CertificatesPath = '/api/v0.1/certificates';
  static readonly postApiV01CertificatesPath = '/api/v0.1/certificates';

  constructor(
    config: __Configuration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param username undefined
   */
  getApiV01CertificatesResponse(username?: string): __Observable<__StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (username != null) __params = __params.set('username', username.toString());
    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/v0.1/certificates`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param username undefined
   */
  getApiV01Certificates(username?: string): __Observable<null> {
    return this.getApiV01CertificatesResponse(username).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param username undefined
   */
  postApiV01CertificatesResponse(username?: string): __Observable<__StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (username != null) __params = __params.set('username', username.toString());
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/certificates`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param username undefined
   */
  postApiV01Certificates(username?: string): __Observable<null> {
    return this.postApiV01CertificatesResponse(username).pipe(
      __map(_r => _r.body as null)
    );
  }
}

module CertificateService {
}

export { CertificateService }
