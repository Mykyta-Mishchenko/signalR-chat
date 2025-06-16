import { HttpClient } from "@angular/common/http";
import { computed, inject, Injectable, signal } from "@angular/core";
import { SignUpModel } from "./models/sign-up.model";
import { finalize, map, Observable } from "rxjs";
import { SignInModel } from "./models/sign-in.model";
import { TokenResponseModel } from "./models/token.model";
import { jwtDecode } from 'jwt-decode';
import { Router } from "@angular/router";
import { environment } from "../../../../environments/environment";
import { ProfileService } from "./profile.service";
import { User } from "./models/user.models";


@Injectable({
    providedIn: 'root'
})
export class AuthService{
  private httpClient = inject(HttpClient);
  private profileSevice = inject(ProfileService);
  private router = inject(Router);
  private apiUrl = environment.apiUrl;

  private user = signal<User | null>(null);
  private accessToken = signal<string | null>(null);

  ACCESS_TOKEN = this.accessToken.asReadonly();
  User = this.user.asReadonly();

  isLoggedIn = computed(() => this.ACCESS_TOKEN() !== null);

  updateUserByToken(accessToken: string | null) {
    this.user.update(user => {
      const updatedUser = this.getUserFromToken(accessToken);
      return { ...updatedUser, profileImgUrl: user?.profileImgUrl } as User;
  });
  }

  setUserProfile() {
    this.profileSevice.getProfileImgUrl(this.user()!.userId).subscribe({
      next:(imageUrl) => {
        this.user.update(user => user ? { ...user, profileImgUrl: imageUrl } : null);
      },
      error: (err) => {
        this.user.update(user => user ? { ...user, profileImgUrl: null } : null);
      }
    })
  }

  signUp(credentials: SignUpModel): Observable<string>{
    return this.httpClient.post<string>(`${this.apiUrl}/auth/signUp`, credentials);
  }
    
  signIn(credentials: SignInModel): Observable<TokenResponseModel>{
    return this.httpClient.post<TokenResponseModel>(`${this.apiUrl}/auth/signIn`, credentials, { withCredentials: true })
      .pipe(map((response: TokenResponseModel) =>
      {
          this.updateUserByToken(response.accessToken);
          this.saveToken(response.accessToken);
          this.setUserProfile();
          return response;
      }));
  }

  refreshToken(): Observable<TokenResponseModel>{
    return this.httpClient.post<TokenResponseModel>(`${this.apiUrl}/auth/refresh`,  {} ,{ withCredentials: true })
      .pipe(map((response: TokenResponseModel) => {
        this.updateUserByToken(response.accessToken);
        this.saveToken(response.accessToken)

        if (!this.user()?.profileImgUrl) {
          this.setUserProfile();
        }

        return response;
      })
    );
  }
    
  logout(): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/auth/logout`, {},{ withCredentials: true }).pipe(
      finalize(() => {
        this.user.set(null);
        this.resetAccessToken();
        this.router.navigateByUrl("/auth/signIn");
      })
    );
  }
    
  getUserFromToken(token :string | null): User | null {
    if (!token) return null;
        
    try {
      const payload: any = jwtDecode(token);

      return {
        userId: payload.nameid,
        userName: payload.unique_name,
        role: payload.role,
        profileImgUrl: null
      }
    } catch (error) {
      console.error("Invalid token", error);
      return null;
    }
  }

  checkTokenValidity(): Promise<string | null> {
    return new Promise((resolve) => {
        this.refreshToken().subscribe({
            next: (response) => resolve(response.accessToken),
            error: () => resolve(null)
        });
    });
}


  public saveToken(accessToken: string): void {
    this.accessToken.update(() => accessToken);
  }

  public resetAccessToken() {
    this.accessToken.update(() => null);
  }
}