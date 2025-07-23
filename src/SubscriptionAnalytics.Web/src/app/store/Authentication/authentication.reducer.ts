import { createReducer, on } from '@ngrx/store';
import { Register, RegisterFailure, RegisterSuccess, login, loginFailure, loginSuccess, logout } from './authentication.actions';
import { User } from './auth.models';

export interface AuthenticationState {
    isLoggedIn: boolean;
    user: User | null;
    error: string | null;
    loading: boolean;
    registerLoading: boolean;
}

const initialState: AuthenticationState = {
    isLoggedIn: false,
    user: null,
    error: null,
    loading: false,
    registerLoading: false,
};

export const authenticationReducer = createReducer(
    initialState,
    on(Register, (state) => ({ ...state, error: null, registerLoading: true })),
    on(RegisterSuccess, (state, { user }) => ({ ...state, isLoggedIn: true, user, error: null, registerLoading: false })),
    on(RegisterFailure, (state, { error }) => ({ ...state, error, registerLoading: false })),

    on(login, (state) => ({ ...state, error: null, loading: true })),
    on(loginSuccess, (state, { user }) => ({ ...state, isLoggedIn: true, user, error: null, loading: false })),
    on(loginFailure, (state, { error }) => ({ ...state, error, loading: false })),
    on(logout, (state) => ({ ...state, user: null, loading: false, registerLoading: false })),


);
