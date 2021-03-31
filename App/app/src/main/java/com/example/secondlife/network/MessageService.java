package com.example.secondlife.network;

import com.example.secondlife.model.Message;
import com.example.secondlife.model.Product;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface MessageService {
    @GET("message/{id}")
    Call<Message> getMessage(@Path("id") int id);

    @GET("message/{idUser}")
    Call<Message> getAllConversations(@Path("idUser") int idUser); // TODO: change type de retour

    @GET("message/{idUser}/{idDest}")
    Call<List<Message>> getConversation(@Path("idUser") int idUser, @Path("idDest") int idDest);

    @POST("message")
    Call<Message> createMessage(@Body Message message);

    @PATCH("message/{id}")
    Call<Message> updateMessage(@Path("id") Integer id, @Body Message message);

    @DELETE("message")
    Call<Message> deleteMessage(@Path("id") Integer id);
}
