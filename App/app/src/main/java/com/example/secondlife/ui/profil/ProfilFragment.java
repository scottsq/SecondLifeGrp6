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
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;

import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentProfilBinding;
import com.example.secondlife.model.User;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.network.UserService;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ProfilFragment extends Fragment {

    private ProfilViewModel profilViewModel;
    private FragmentProfilBinding binding;
    View view;

    Gson gson = new GsonBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ssZ")
            .create();
    private final Retrofit retrofit = new Retrofit.Builder()
            .baseUrl("http://10.0.2.2:61169/api/")
            .client(OkHttpClass.getUnsafeOkHttpClient())
            .addConverterFactory(GsonConverterFactory.create())
            .build();

    UserService apiService = retrofit.create(UserService.class);
    private User user = null;

    @Override
    public View onCreateView (LayoutInflater inflater,
                              ViewGroup container,
                              Bundle savedInstanceState) {

        binding = FragmentProfilBinding.inflate(inflater, container, false);
        view = binding.getRoot();
        int id = 1;

        // Edit button
        int idEditButton = getResources().getIdentifier("editButton", "id", getActivity().getPackageName());
        Button editButton = view.findViewById(idEditButton);
        editButton.setOnClickListener( new View.OnClickListener(){
            @Override
            public void onClick(View view){
                setEnabled(new String[]{"editTextPersonName","editTextEmail","editTextAvatarUrl"});
            }
        });

        // Save button
        int idSaveButton = getResources().getIdentifier("saveButton", "id", getActivity().getPackageName());
        Button saveButton = view.findViewById(idSaveButton);
        saveButton.setOnClickListener( new View.OnClickListener(){
            @Override
            public void onClick(View view){
                user.setName(((EditText) getViewById("editTextPersonName")).getText().toString());
                user.setEmail(((EditText) getViewById("editTextEmail")).getText().toString());
                user.setAvatarUrl(((EditText) getViewById("editTextAvatarUrl")).getText().toString());

                apiService.updateUser(id, user).enqueue(new Callback<User>() {
                    @Override
                    public void onResponse(Call<User> call, Response<User> response) {
                        user = response.body();
                        Log.v("test user update" , "test update");
                        //Log.v("test user update" , user.getEmail());
                    }

                    @Override
                    public void onFailure(Call<User> call, Throwable t) {
                        Log.i("test","fail");
                        t.printStackTrace();

                    }
                });

            }
        });

        apiService.getUser(id).enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                user = response.body();
                Log.v("test user" , "test");
                Log.v("Name Profil: ",user.getName());
                //Log.v("Name Email: ",user.getEmail());
                Log.v("Login Profil:",user.getLogin());
                setTextView("TextViewName", user.getName());
                setTextEditText("editTextPersonName", user.getName() == null?"":user.getName());
                setTextEditText("editTextEmail",user.getEmail() == null?"":user.getEmail());
                setTextEditText("editTextAvatarUrl",user.getAvatarUrl() == null?"":user.getAvatarUrl());
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                Log.i("test","fail");
                t.printStackTrace();

            }
        });

        return view;
    }

    public void setTextView(String idTextView, String text) {

        View view = getViewById(idTextView);
        ((TextView) view).setText(text);
    }

    public void setTextEditText(String idEditView, String text) {

        View view = getViewById(idEditView);
        ((EditText) view).setText(text);
    }

    public void setEnabled(String[] listId){
        for (int i = 0; i < listId.length; i++) {
            View view = getViewById(listId[i]);
            view.setEnabled(!view.isEnabled());
        }
    }

    public View getViewById(String idView) {
        int id = getResources().getIdentifier(idView, "id", getActivity().getPackageName());

        return view.findViewById(id);
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}