import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';


import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';


import { HttpErrorHandler, HandleError } from '../http-error-handler.service';
import { User } from './User';
import { getBaseUrl } from '../../main';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': 'https://localhost:44316',
    'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS',
    'Access-Control-Allow-Credentials': 'true',
    'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token'
  })
};

@Injectable({
  providedIn: 'root'  // <- ADD THIS
})
export class UsersService {
  heroesUrl = 'https://localhost:44316/api/users';  // URL to web api
  private handleError: HandleError;


  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler) {
    this.handleError = httpErrorHandler.createHandleError('PeopleService');
  }

  /** GET heroes from the server */
  getHeroes(): Observable<User[]> {
    return this.http.get<User[]>(this.heroesUrl)
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }

  /** POST: add a new hero to the database */
  addHero(hero: User): Observable<User> {
    return this.http.post<User>(this.heroesUrl, hero, httpOptions)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }

  /** POST: add a new hero to the server */
  addHeroX(hero: User): Observable<User> {
    return this.http.post<User>(this.heroesUrl, hero, httpOptions)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }


  /** PUT: update the hero on the server. Returns the updated hero upon success. */
  updateHero(hero: User): Observable<User> {
    // httpOptions.headers =
    //   httpOptions.headers.set('Authorization', 'my-new-auth-token');
    const url = `${this.heroesUrl}/${hero.id}`; // PUT api/heroes/42
    return this.http.put<User>(url, hero, httpOptions)
      .pipe(
        catchError(this.handleError('updateHero', hero))
      );
  }

  /** DELETE: delete the hero from the server */
  deleteHero(id: number): Observable<{}> {
    const url = `${this.heroesUrl}/${id}`; // DELETE api/heroes/42
    return this.http.delete(url, httpOptions)
      .pipe(
        catchError(this.handleError('deleteHero'))
      );
  }
}

