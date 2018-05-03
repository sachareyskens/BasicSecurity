package be.pxl.repository;

import be.pxl.entity.Message;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface MessageRepository extends JpaRepository<Message, Integer> {
    @Query("from Message m where m.reciever =:username")
    List<Message> findByUsername(@Param("username") String username);
    @Query("from Message m where m.sender= :sender and m.reciever=:reciever")
    List<Message> findBySenderAndReciever(@Param("sender") String sender, @Param("reciever") String reciever);
    @Query("select count(*) from Message m")
    Integer countAll();
}
