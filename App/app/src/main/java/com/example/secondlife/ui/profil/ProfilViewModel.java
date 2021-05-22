package com.example.secondlife.ui.profil;

import android.util.Log;

import com.example.secondlife.LocalData;
import com.example.secondlife.model.User;
import com.example.secondlife.network.UserService;

import java.util.ArrayList;
import java.util.List;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class ProfilViewModel extends ViewModel {

    private LocalData mLocalData = LocalData.GetInstance();
    private Retrofit mRetrofit = mLocalData.GetInstance().GetRetrofit();
    private UserService mApiService = mRetrofit.create(UserService.class);
    private MutableLiveData<User> mUserLiveData;

    public ProfilViewModel() {
        if (mUserLiveData == null) mUserLiveData = new MutableLiveData<>();
        getApiUser();
    }

    private void getApiUser() {
        mApiService.getUser(mLocalData.getToken(), mLocalData.getUserId()).enqueue(patchUser());
    }

    private Callback<User> patchUser() {
        return new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                Log.v("TESTT", "response.body()");
                mUserLiveData.setValue(response.body());

            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                Log.i("test","fail");
                t.printStackTrace();
            }
        };
    }

    public void updateUser(User user) {
        List<String> fields = new ArrayList<>();
        fields.add("name");
        fields.add("email");
        fields.add("avatarUrl");
        mApiService.updateUser(mLocalData.getToken(), mLocalData.getUserId(), mLocalData.ObjectToPatch(user, fields)).enqueue(patchUser());
    }

    public MutableLiveData<User> getUserLiveData() {
        return mUserLiveData;
    }
}