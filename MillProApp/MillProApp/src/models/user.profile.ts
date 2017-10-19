export class UserProfile {

  constructor(
    public Id: string,
    public firstName: string,
    public lastName: string,
    public companyName: string,
    public companyEmail: string,
    public email: string,
    public password: string,
    public confirmPassword: string
    )
    { }
}
