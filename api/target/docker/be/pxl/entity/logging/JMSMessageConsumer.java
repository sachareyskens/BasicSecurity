package be.pxl.entity.logging;


import be.pxl.service.ILogService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jms.annotation.JmsListener;
import org.springframework.stereotype.Component;

import javax.jms.JMSException;
import javax.jms.TextMessage;

@Component
public class JMSMessageConsumer {
    @Autowired
    private ILogService service;

    @JmsListener(destination = "LoggerQueue")
    public void onMessage(TextMessage message) {

        Log log = new Log();
        try {
            log.setLog(message.getText());

        } catch (JMSException e) {
            log.setLog("Log failed: " + e.getMessage());
        } finally {
            service.persist(log);
        }

    }
}
