package be.pxl.repository;

import be.pxl.entity.Message;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface MessageRepository extends JpaRepository<Message, Integer> {
    @Query("from Message m where m.reciever =:username")
    public List<Message> findByUsername(@Param("username") String username);
}
