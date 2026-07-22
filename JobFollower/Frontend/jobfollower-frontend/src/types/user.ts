export interface UserDto {
    userId: number;
    name: string;
    email: string;
}

export interface RegisterUserDto {
    name: string;
    email: string;
    password: string;
}

export interface LoginDto {
    email: string;
    password: string;
}

export interface LoginResponseDto {
    accessToken: string;
    user: UserDto;
}