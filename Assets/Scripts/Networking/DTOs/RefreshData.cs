using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshData
{
    public string expires_in;
    public string token_type;
    public string refresh_token;
    public string id_token;
    public string user_id;
    public string project_id;

    public LoginData ToLoginData()
    {
        var result = new LoginData();
        result.idToken = this.id_token;
        result.refreshToken = this.refresh_token;
        result.expiresIn = this.expires_in;
        result.localId = this.user_id;

        return result;
    }
}
