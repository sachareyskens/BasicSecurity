package be.pxl.entity.logging;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.stereotype.Component;

import javax.jms.Queue;

@Component
public class JMSMessageLogger {

    @Autowired
    private JmsTemplate jmsTemplate;

    @Autowired
    @Qualifier("JMSLoggerQueue")
    private Queue destination;

    public void log(String log) {
        jmsTemplate.send(destination,
                session -> session.createTextMessage(log)
        );
    }
}
