package be.pxl.entity;

public class StatsSource {
    private String totalUsers;
    private String totalMessages;

    public StatsSource() {
    }

    public String getTotalUsers() {
        return totalUsers;
    }

    public void setTotalUsers(String totalUsers) {
        this.totalUsers = totalUsers;
    }

    public String getTotalMessages() {
        return totalMessages;
    }

    public void setTotalMessages(String totalMessages) {
        this.totalMessages = totalMessages;
    }
}
