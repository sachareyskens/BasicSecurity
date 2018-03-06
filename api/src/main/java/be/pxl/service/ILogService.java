package be.pxl.service;

import be.pxl.entity.logging.Log;
import org.springframework.beans.factory.annotation.Configurable;

import java.util.List;

@Configurable
public interface ILogService {
    Log find(int id);
    List<Log> All();
    void persist(Log l);
    void update(Log l);
    void delete(int id);
}
