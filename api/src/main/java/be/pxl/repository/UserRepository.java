package be.pxl.repository;

import be.pxl.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface UserRepository extends JpaRepository<User, String> {
    @Query("from User u where u.accesstoken =:accesstoken")
    User findLoggedIn(@Param("accesstoken") String accesstoken);

    @Query("select count(*) from User u")
    Integer countAll();

    @Query("select username from User u")
    List<String> getAllNames();
}
