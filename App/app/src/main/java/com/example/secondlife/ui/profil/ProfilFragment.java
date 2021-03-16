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


    @Override
    public View onCreateView (LayoutInflater inflater,
                              ViewGroup container,
                              Bundle savedInstanceState) {

        binding = FragmentProfilBinding.inflate(inflater, container, false);
        view = binding.getRoot();

        int idButton = getResources().getIdentifier("editButton", "id", getActivity().getPackageName());
        Button editButton = view.findViewById(idButton);
        editButton.setOnClickListener( new View.OnClickListener(){
            @Override
            public void onClick(View view){
                setEnabled(new String[]{"editTextPersonName","editTextEmail","editTextAvatarUrl"});
            }
        });


        UserService apiService = retrofit.create(UserService.class);
        int id = 1;
        apiService.getUser(id).enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                User user = response.body();
                Log.v("test user" , "test");
                Log.v("Name Profil: ",user.getName());
                //Log.v("Name Email: ",user.getEmail());
                Log.v("Login Profil:",user.getLogin());
                setTextTextView("TextViewName", user.getName());
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

    public void setTextTextView(String idTextView, String text) {

        int id = getResources().getIdentifier(idTextView, "id", getActivity().getPackageName());
        TextView myTextView = view.findViewById(id);

        myTextView.setText(text);
    }

    public void setTextEditText(String idTextView, String text) {

        int id = getResources().getIdentifier(idTextView, "id", getActivity().getPackageName());
        EditText myEditText = view.findViewById(id);

        myEditText.setText(text);
    }

    public void setEnabled(String[] listId){
        for (int i = 0; i < listId.length; i++) {
            int id = getResources().getIdentifier(listId[i], "id", getActivity().getPackageName());
            View myEditText = view.findViewById(id);

            myEditText.setEnabled(!myEditText.isEnabled());
        }
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}