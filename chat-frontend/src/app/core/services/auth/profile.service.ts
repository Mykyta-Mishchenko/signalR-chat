import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, map, Observable, of, shareReplay } from "rxjs";
import { environment } from "../../../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class ProfileService {
    private httpClient = inject(HttpClient);
    private profileUrls = new Map<number, Observable<string>>();
    private apiUrl = environment.apiUrl;

    private getUserProfile(userId: number): Observable<string> {
        return this.httpClient.get(`${this.apiUrl}/profile/image/${userId}`, {
            withCredentials: true,
            responseType: 'blob'
        }).pipe(
            map(response => {
                if (response.size == 0) {
                    return "";
                }
                return URL.createObjectURL(response)
            })
        );
    }

    getProfileImgUrl(userId: number): Observable<string> {
        if (!this.profileUrls.has(userId)) {
          const url$ = this.getUserProfile(userId).pipe(
            map(imgUrl => imgUrl),
            catchError(() => of('empty-profile.png')),
            shareReplay(1)
          );
          this.profileUrls.set(userId, url$);
        }
        return this.profileUrls.get(userId)!;
    }

    setUserProfile(formData: FormData) : Observable<any>{
        return this.httpClient.post(`${this.apiUrl}/profile/upload`, formData, {
          withCredentials: true
        });
    }
}