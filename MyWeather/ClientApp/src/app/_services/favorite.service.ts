import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { City } from '../_models/city.model';

@Injectable()
export class FavoriteService {
    baseUrl = environment.WebApiUri;

    constructor(private http: HttpClient) { }

    getFavoriteCities() {
        return this.http.get<City[]>(this.baseUrl + 'Favorites/GetFavoriteCities');
    }

    addFavorite(key: string) {
        return this.http.post(this.baseUrl + 'Favorites/AddFavorite/?key=' + key, {});
    }

    deleteFavorite(key: string) {
        return this.http.delete(this.baseUrl + 'Favorites/DeleteFavorite/?key=' + key, {});
    }
}
