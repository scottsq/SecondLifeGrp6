package com.example.secondlife.ui.home;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.example.secondlife.repository.Repository;

public class HomeViewModel extends ViewModel {

    private MutableLiveData<String> mText;
    private final Repository repository = new Repository(); // Exemple

    public HomeViewModel() {
        mText = new MutableLiveData<>();
        mText.setValue("This is home fragment");
    }

    //public  getLatestGames (){
    //    return repository.getLatestGame();
    //} // Exemple

    public LiveData<String> getText()
    {
        return mText;
    }
}