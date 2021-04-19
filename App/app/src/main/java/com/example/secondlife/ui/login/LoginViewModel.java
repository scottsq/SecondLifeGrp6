package com.example.secondlife.ui.login;

import android.util.Log;
import android.view.View;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.model.LoginResponse;
import com.example.secondlife.model.User;
import com.example.secondlife.network.UserService;
import com.google.android.material.bottomnavigation.BottomNavigationView;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class LoginViewModel extends ViewModel {

    private Retrofit mRetrofit = LocalData.GetInstance().GetRetrofit();
    private MutableLiveData<Boolean> isConnected;

    public LoginViewModel() {
        if (isConnected == null) isConnected = new MutableLiveData<>();
    }

    public MutableLiveData<Boolean> getIsConnected() {
        return isConnected;
    }

    public void Login(String login, String pass) {
        UserService apiService = mRetrofit.create(UserService.class);
        User u = new User();
        u.setLogin(login);
        u.setPassword(pass);
        apiService.loginUser(u).enqueue(postLogin());
    }

    private Callback<LoginResponse> postLogin() {
        return new Callback<LoginResponse>() {
            @Override
            public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {
                LoginResponse check = response.body();

                if (response.isSuccessful())
                {
                    // Save values
                    LocalData localData = LocalData.GetInstance();
                    localData.setUserId(check.getId());
                    localData.setToken(check.getToken());
                    isConnected.setValue(true);
                }
                else isConnected.setValue(false);
            }

            @Override
            public void onFailure(Call<LoginResponse> call, Throwable t) {
                // Juste pour test vu que j'ai pas accès à l'api TwT
                // Faudra mettre un message d'erreur
                LocalData.GetInstance().setUserId(0);
                // TODO: change to false
                isConnected.setValue(true);

                // Log error here since request failed
                Log.i("test","fail");
                t.printStackTrace();
            }
        };
    }
}