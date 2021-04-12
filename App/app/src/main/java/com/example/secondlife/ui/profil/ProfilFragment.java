package com.example.secondlife.ui.profil;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentProfilBinding;
import com.example.secondlife.model.User;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.network.UserService;
import com.example.secondlife.ui.login.LoginFragment;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import org.json.JSONException;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ProfilFragment extends Fragment {

    private ProfilViewModel profilViewModel;
    private FragmentProfilBinding binding;
    private View view;
    private LocalData localData = LocalData.GetInstance();
    private final Retrofit retrofit = localData.GetRetrofit();
    private UserService apiService = retrofit.create(UserService.class);
    private User user = null;

    @Override
    public View onCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        binding = FragmentProfilBinding.inflate(inflater, container, false);
        view = binding.getRoot();

        binding.editButton.setOnClickListener(view -> {
            binding.editTextPersonName.setEnabled(true);
            binding.editTextEmail.setEnabled(true);
            binding.editTextAvatarUrl.setEnabled(true);
        });

        // Save button
        Button saveButton = binding.saveButton;
        saveButton.setOnClickListener(callSaveButton());

        // Disconnection button
        Button disconnectionButton = binding.disconnectionButton;
        disconnectionButton.setOnClickListener(callDisconnectionButton());

        // Save values
        apiService.getUser(localData.getToken(),localData.getUserId()).enqueue(patchUser());

        return view;
    }

    private Callback<User> patchUser() {
        return new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                user = response.body();
                binding.editTextPersonName.setText(user.getName() == null ? "" : user.getName());
                binding.editTextEmail.setText(user.getEmail() == null ? "" :user.getEmail());
                binding.editTextAvatarUrl.setText(user.getAvatarUrl() == null ? "" : user.getAvatarUrl());
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                Log.i("test","fail");
                t.printStackTrace();
            }
        };
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

    public View.OnClickListener callSaveButton() {
        return view -> {
            user.setName(binding.editTextPersonName.getText().toString());
            user.setEmail(binding.editTextEmail.getText().toString());
            user.setAvatarUrl(binding.editTextAvatarUrl.getText().toString());
            apiService.updateUser(localData.getToken(), localData.getUserId(), localData.ObjectToPatch(user)).enqueue(patchUser());

        };
    }

    public View.OnClickListener callDisconnectionButton() {
        return view -> {
            localData.setUserId(-1);
            localData.setToken("");

            BottomNavigationView navigation = (BottomNavigationView) getActivity().findViewById(R.id.nav_view);
            navigation.getMenu().clear();
            navigation.inflateMenu(R.menu.bottom_nav_menu);
            navigation.setSelectedItemId(R.id.navigation_login);
        };
    }

}